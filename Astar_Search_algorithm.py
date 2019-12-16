import pygame

import math



class Node:

    def __init__(self):

        self.mark = False

        self.g = 0

        self.f = 0

        self.parent = Position(0, 0)



class Position:

    def __init__(self, left, top):

        self.left=left

        self.top=top







start_position = Position(300, 100)

dest_position = Position(20, 300)

distance=[5, 7, 5, 7, 5, 7, 5, 7]

list_wait = []

next_node = [Position(0, -5), Position(5, -5), Position(5, 0), Position(5, 5), 

    Position(0, 5), Position(-5, 5), Position(-5, 0), Position(-5, -5)]







pygame.init()



win = pygame.display.set_mode((500, 500))

pygame.display.set_caption("A Star Search")







virtual_map = [[0] * 500 for i in range(500)]

pixcel = pygame.image.load("E:\TAI LIEU MON HOC\Python\Map.png")

for i in range(0, 500):

    for j in range(0, 500):

        if (pixcel.get_at((j, i))==(255, 255, 255)):

            virtual_map[i][j] = True

        else:

            virtual_map[i][j] = False





nodes = [[0] * 500 for i in range(500)]

for i in range(0,500):

    for j in range(0, 500):

        nodes[i][j]= Node()





win.blit(pixcel,(0,0))

pygame.draw.circle(win, (255,0,0), (start_position.top, start_position.left), 5)

pygame.draw.circle(win, (0,255,0), (dest_position.top, dest_position.left), 5)

pygame.display.update()







def show_path(position):

    list_path = []

    position = nodes[position.top][position.left].parent

    

    while position.top != start_position.top or position.left != start_position.left:

        list_path.append(position)

        position = nodes[position.top][position.left].parent



    for i in list_path:

        pygame.draw.circle(win, (255,245,0), (i.top, i.left), 2)

    pygame.display.update()





def get_min():

    position_min = list_wait[0]

    for tmp in list_wait:

        if nodes[tmp.top][tmp.left].f < nodes[position_min.top][position_min.left].f:

            position_min = tmp



    return position_min





def is_in_list_wait(position):

    for wait in list_wait:

        if position.left == wait.left and position.top == wait.top:

            return True

            break

    return False





def heuristic (left, top):

    return math.sqrt((left-dest_position.left)*(left-dest_position.left)+(top-dest_position.top)*(top-dest_position.top))







def a_star ():

    global list_wait

    global nodes

    list_wait.append(start_position)



    while list_wait:

        current_position = get_min()

        list_wait.remove(current_position)

        nodes[current_position.top][current_position.left].mark=True



        if current_position.top == dest_position.top and current_position.left == dest_position.left:

            show_path(current_position)

            break



        for i in range(0, 8):

            tmp_left = current_position.left + next_node[i].left

            tmp_top = current_position.top + next_node[i].top



            if tmp_left >= 0 and tmp_left < 500 and tmp_top >= 0 and tmp_top < 500 and virtual_map[tmp_left][tmp_top] is True:

                if nodes[tmp_top][tmp_left].mark is False:



                    tmp_g = nodes[current_position.top][current_position.left].g + distance[i]

                    tmp_f = tmp_g + heuristic(tmp_left, tmp_top)



                    if is_in_list_wait(current_position) is False:

                        nodes[tmp_top][tmp_left].parent = current_position

                        nodes[tmp_top][tmp_left].g = tmp_g

                        nodes[tmp_top][tmp_left].f = tmp_f

                        list_wait.append(Position(tmp_left, tmp_top))



                    elif tmp_f < nodes[tmp_top][tmp_left].f:

                        nodes[tmp_top][tmp_left].parent = current_position

                        nodes[tmp_top][tmp_left].g = tmp_g

                        nodes[tmp_top][tmp_left].f = tmp_f

                   



a_star ()





run=True

while run:

    pygame.time.delay(10)

    for event in pygame.event.get():

        if event.type == pygame.QUIT:

            run=False



pygame.quit()